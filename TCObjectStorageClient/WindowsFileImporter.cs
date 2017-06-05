using System.Collections.Generic;
using Microsoft.Win32;
using TCObjectStorageClient.Interfaces;

namespace TCObjectStorageClient
{
    public class WindowsFileImporter : IFileImporter
    {
        public IList<string> GetFilePathList()
        {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            var isShow = dialog.ShowDialog();
            if (isShow.HasValue && isShow.Value)
            {
                return dialog.FileNames;
            }
            return new List<string>();
        }
    }
}
