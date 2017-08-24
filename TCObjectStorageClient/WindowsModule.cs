using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            builder.RegisterInstance<IFileImporter>(new WindowsFileImporter());
            builder.RegisterInstance<IAlertDialog>(new WindowsAlertDialog(_parent));
            builder.RegisterInstance<IPreferences>(new WindowsPreferences());
        }
    }
}
