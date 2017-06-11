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

            if (filePathList.Count <= 0) return;

            _alertDialog.ShowAlert(filePathList.Aggregate((s1, s2) => s1 + "\n" + s2), async isOk =>
            {
                if (!isOk) return;

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var client = new TCObjectStorage();
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

                var tasks = filePathList
                    .Select(filePath =>
                        client.UploadFile(token, Account, ContainerName, Path.GetFileName(filePath), File.ReadAllBytes(filePath)))
                    .ToList();
                var isSuccess = (await Task.WhenAll(tasks)).All(_ => _);
                _alertDialog.ShowAlert($"{(isSuccess ? "Success" : "Fail")} to upload files.\n" +
                                       $"Elapsed time is {stopwatch.ElapsedMilliseconds} ms");
            });
        }

        public async void UploadDirectory()
        {
            var dirInfo = _fileImporter.GetDirectoryInfo();
            if (dirInfo == null)
                return;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var entity = new DirectoryEntity(dirInfo.FullName);
            var children = entity.GetAllChildren();

            var client = new TCObjectStorage();
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

            var tasks = children
                .Select(child =>
                    client.UploadFile(token, Account, ContainerName, child.pathFromBase.Replace('\\', '/'), File.ReadAllBytes(child.entity.Path)))
                .ToList();
            var isSuccess = (await Task.WhenAll(tasks)).All(_ => _);
            stopwatch.Stop();
            _alertDialog.ShowAlert($"{(isSuccess ? "Success" : "Fail")} to upload files.\n" +
                                   $"Elapsed time is {stopwatch.ElapsedMilliseconds} ms");
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
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var client = new TCObjectStorage();
            var token = await client.PostToken(TenentName, UserName, Password);

            var hasContainer = await client.ContainsContainer(token, Account, ContainerName);
            if (!hasContainer)
            {
                _alertDialog.ShowAlert($"Invalid [{ContainerName}] container");
                return;
            }

            var result = await GetFilesInContainer();
            var files = result.files;

            var tasks = files
                .Select(file => client.DeleteFile(token, Account, ContainerName, file))
                .ToList();
            var isSuccess = (await Task.WhenAll(tasks)).All(_ => _);

            stopWatch.Stop();
            if (isSuccess)
            {
                _alertDialog.ShowAlert($"Success to delete files.\n" +
                                       $"Elapsed time is {stopWatch.ElapsedMilliseconds} ms");
            }
            else
            {
                _alertDialog.ShowAlert($"Fail to delete files\n" +
                                       $"Elapsed time is {stopWatch.ElapsedMilliseconds} ms");
            }
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
