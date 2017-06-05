using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TCObjectStorageClient.ViewModels;

namespace TCObjectStorageClient
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var mainViewModel = new MainViewModel(new WindowsFileImporter(), new WindowsAlertDialog(this), new WindowsPreferences());
            this.DataContext = mainViewModel;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox box)
            {
                var dataContext = this.DataContext;
                if (dataContext is MainViewModel vm)
                {
                    vm.Password = box.Password;
                }
            }
        }
    }

    public class OpenFileCommand : ICommand
    {
        private readonly MainViewModel _viewModel;

        public OpenFileCommand(MainViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            await _viewModel.UploadFiles();
        }

        public event EventHandler CanExecuteChanged;
    }

    namespace ViewModels
    {
        public partial class MainViewModel
        {
            public ICommand OpenFileCommand => new OpenFileCommand(this);
        }
    }
}