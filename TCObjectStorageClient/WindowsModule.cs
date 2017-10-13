using System.Windows;
using Autofac;
using TCObjectStorageClient.Interfaces;
using TCObjectStorageClient.IO;

namespace TCObjectStorageClient
{
    internal class WindowsModule : Module
    {
        private readonly Window _parent;

        public WindowsModule(Window parent)
        {
            _parent = parent;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var alertDialog = new WindowsAlertDialog(_parent);
            var prefrences = new WindowsPreferences();
            var fileImporter = new WindowsFileImporter(prefrences);

            builder.RegisterInstance<IFileImporter>(fileImporter);
            builder.RegisterInstance<IAlertDialog>(alertDialog);
            builder.RegisterInstance<IPreferences>(prefrences);
        }
    }
}
