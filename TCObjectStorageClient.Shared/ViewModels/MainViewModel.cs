using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using TCObjectStorageClient.Annotations;
using TCObjectStorageClient.Interfaces;
using TCObjectStorageClient.IO;
using TCObjectStorageClient.Models;
using IContainer = Autofac.IContainer;
using ToastCloud.ObjectStorage;

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
        private string _containerName;

        private readonly ObjectStorage _objectStorage;

        public MainViewModel(IContainer container) 
            : this(container.Resolve<IFileImporter>(), container.Resolve<IAlertDialog>(), container.Resolve<IPreferences>())
        {
            
        }

        public MainViewModel(IFileImporter fileImporter, IAlertDialog alertDialog, IPreferences preferences)
        {
            _fileImporter = fileImporter;
            _alertDialog = alertDialog;
            _preferences = preferences;

            _objectStorage = new ObjectStorage();

            TenentName = _preferences.GetString(Constants.TenentNameKey, string.Empty);
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
        
        public string ContainerName
        {
            get => _containerName;
            set => OnPropertyChanged(ref _containerName, value, Constants.ContainerNameKey);
        }

        private async Task<bool> HasContainer()
        {
            var result = await _objectStorage.ContainsContainer(ContainerName);
            var hasContainer = result.IsSuccess ? result.Value : false;
            if (!hasContainer)
                _alertDialog.ShowAlert($"Invalid [{ContainerName}] container");
            return hasContainer;
        }

        private async Task<bool> Authenticate()
        {
            if (_objectStorage.IsAuthenticate)
                return true;

            var result = await _objectStorage.Authenticate(TenentName, UserName, Password);
            return result.IsSuccess ? result.Value : false;
        }

        private async Task<bool> AwaitForProgress<T>(List<Task<T>> tasks, Action<float> onProgress)
        {
            var isSuccessList = new bool[tasks.Count];
            while (isSuccessList.Count(_ => _) < tasks.Count)
            {
                for (int i = 0; i < tasks.Count; i++)
                {
                    if (tasks[i].IsCompleted && !isSuccessList[i])
                    {
                        isSuccessList[i] = true;
                        onProgress?.Invoke((float)isSuccessList.Count(_ => _) / tasks.Count);
                    }
                }
                await Task.Delay(TimeSpan.FromMilliseconds(1));
            }
            return isSuccessList.All(_ => _);
        }

        public void UploadFiles(Action<float> onProgress)
        {
            var filePathList = _fileImporter.GetFilePathList();

            if (filePathList.Count <= 0) return;

            string alertMessage;
            if (filePathList.Count <= 10)
                alertMessage = filePathList.Aggregate((s1, s2) => s1 + "\n" + s2);
            else
                alertMessage = $"Upload {filePathList.Count} files";

            _alertDialog.ShowAlert(alertMessage, async isOk =>
            {
                if (!isOk) return;

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var isAuthenticate = await Authenticate();
                if (!isAuthenticate)
                {
                    _alertDialog.ShowAlert("Failed to get a token");
                    return;
                }

                if (!await HasContainer())
                    return;

                var tasks = filePathList
                    .Select(filePath =>
                        _objectStorage.UploadFile(ContainerName, Path.GetFileName(filePath), new FileStream(filePath, FileMode.Open)))
                    .ToList();
                var isSuccess = await AwaitForProgress(tasks, onProgress);
                _alertDialog.ShowAlert($"{(isSuccess ? "Success" : "Fail")} to upload files.\n" +
                                       $"Elapsed time is {stopwatch.ElapsedMilliseconds} ms");
            });
        }

        public async void UploadDirectory(Action<float> onProgress)
        {
            var dirInfo = _fileImporter.GetDirectoryInfo();
            if (dirInfo == null)
                return;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var entity = new DirectoryEntity(dirInfo.FullName);
            var children = entity.GetAllChildren();

            var isAuthenticate = await Authenticate();
            if (!isAuthenticate)
            {
                _alertDialog.ShowAlert("Failed to get a token");
                return;
            }

            if (!await HasContainer())
                return;

            _alertDialog.ShowAlert("Do you want to upload file under directory", async isOk =>
            {
                var tasks = children.Select(child =>
                    _objectStorage.UploadFile(ContainerName,
                        child.pathFromBase.Replace(Path.DirectorySeparatorChar, '/'), new FileStream(child.entity.Path, FileMode.Open))).ToList();

                var isSuccess = await AwaitForProgress(tasks, onProgress);
                stopwatch.Stop();

                _alertDialog.ShowAlert($"{(isSuccess ? "Success" : "Fail")} to upload files.\n" +
                                       $"Elapsed time is {stopwatch.ElapsedMilliseconds} ms");
            });
        }

        public async void GetFiles()
        {
            await GetFilesInContainer();
        }

        private async Task<(bool isSuccess, List<string> files)> GetFilesInContainer()
        {
            await Authenticate();
            var result = await _objectStorage.FileList(ContainerName);
            if (result.IsSuccess)
                return (true, result.Value.Files.Select(i => i.Name).ToList());

            return (false, null);
        }

        public async void DeleteAllFilesInContainer(Action<float> onProgress)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var isAuthenticate = await Authenticate();

            if (!isAuthenticate)
                return;

            var result = await GetFilesInContainer();
            var files = result.files;

            var tasks = files
                .Select(file => _objectStorage.DeleteFile(ContainerName, file))
                .ToList();
            var isSuccess = await AwaitForProgress(tasks, onProgress);

            stopWatch.Stop();
            _alertDialog.ShowAlert($"{(isSuccess ? "Success" : "Fail")} to delete files.\n" +
                                   $"Elapsed time is {stopWatch.ElapsedMilliseconds} ms");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged<T>(ref T target, T value, string cacheKey = null, [CallerMemberName] string propertyName = null)
        {
            target = value;
            if (cacheKey != null)
            {
                if (target is string)
                    _preferences.SetString(cacheKey, value as string);
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
