using System;
using System.Windows;
using TCObjectStorageClient.Interfaces;

namespace TCObjectStorageClient
{
    public class WindowsAlertDialog : IAlertDialog
    {
        private readonly Window _parent;

        public WindowsAlertDialog(Window parent)
        {
            _parent = parent;
        }

        public void ShowAlert(string alertText)
        {
            MessageBox.Show(_parent, alertText);
        }

        public void ShowAlert(string alertText, Action<bool> callback)
        {
            var result = MessageBox.Show(_parent, alertText, "AlertDialog", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                callback(true);
            }
            else if(result == MessageBoxResult.No)
            {
                callback(false);
            }
        }
    }
}
