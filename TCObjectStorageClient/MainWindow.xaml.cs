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

    public class DelegateCommand : CommandBase
    {
        private readonly Action _action;

        public DelegateCommand(MainViewModel viewModel, Action action) : base(viewModel)
        {
            _action = action;
        }

        protected override void ExecuteInternal()
        {
            _action();
        }
    }

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