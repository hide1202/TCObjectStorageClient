using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        public MainViewModel(IFileImporter fileImporter, IAlertDialog alertDialog, IPreferences preferences)
        {
            _fileImporter = fileImporter;
            _alertDialog = alertDialog;
            _preferences = preferences;

            TenentName = _preferences.GetString(Constants.TenentNameKey, string.Empty);
            Account = _preferences.GetString(Constants.AccountKey, string.Empty);
            UserName = _preferences.GetString(Constants.UserNameKey, string.Empty);
        }

        public string TenentName
        {
            get => _tenentName;
            set
            {
                _tenentName = value;
                _preferences.SetString(Constants.TenentNameKey, value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TenentName"));
            }
        }

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                _preferences.SetString(Constants.UserNameKey, value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UserName"));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Password"));
            }
        }

        public string Token
        {
            get => _token;
            set
            {
                _token = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Token"));
            }
        }

        public string Account
        {
            get => _account;
            set
            {
                _account = value;
                _preferences.SetString(Constants.AccountKey, value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Account"));
            }
        }

        public async Task UploadFiles()
        {
            var filePathList = _fileImporter.GetFilePathList();
            if (filePathList.Count > 0)
            {
                _alertDialog.ShowAlert(filePathList.Aggregate((s1, s2) => s1 + ", " + s2), async isOk =>
                {
                    if (isOk)
                    {
                        TCObjectStorage client = new TCObjectStorage();
                        var token = await client.PostToken(TenentName, UserName, Password);

                        var isSuccess = await client.UploadFile(token, Account, "Images", Path.GetFileName(filePathList[0]), File.ReadAllBytes(filePathList[0]));
                        _alertDialog.ShowAlert(isSuccess.ToString());
                    }
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
