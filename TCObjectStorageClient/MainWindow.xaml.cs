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

    public abstract class CommandBase : ICommand
    {
        protected readonly MainViewModel _viewModel;

        public CommandBase(MainViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            ExecuteInternal();
        }

        public event EventHandler CanExecuteChanged;

        protected abstract void ExecuteInternal();
    }

    public class OpenFileCommand : CommandBase
    {
        public OpenFileCommand(MainViewModel viewModel) : base(viewModel)
        {
        }

        protected override void ExecuteInternal()
        {
            _viewModel.UploadFiles();
        }
    }

    public class OpenDirectoryCommand : CommandBase
    {
        public OpenDirectoryCommand(MainViewModel viewModel) : base(viewModel)
        {
        }

        protected override void ExecuteInternal()
        {
            _viewModel.UploadDirectory();
        }
    }

    namespace ViewModels
    {
        public partial class MainViewModel
        {
            public ICommand OpenFileCommand => new OpenFileCommand(this);

            public ICommand OpenDirectoryCommand => new OpenDirectoryCommand(this);
        }
    }
}