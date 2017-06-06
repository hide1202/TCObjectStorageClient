using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using TCObjectStorageClient.Interfaces;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

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

        public DirectoryInfo GetDirectoryInfo()
        {
            var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                return new DirectoryInfo(dialog.SelectedPath);
            }
            return null;
        }
    }
}
