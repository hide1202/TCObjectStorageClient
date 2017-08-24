using System.Windows;
using System.Windows.Controls;
using Autofac;
using TCObjectStorageClient.Interfaces;
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

            var builder = new ContainerBuilder();
            builder.RegisterModule(new WindowsModule(this));
            var container = builder.Build();

            var mainViewModel = new MainViewModel(container);
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
}