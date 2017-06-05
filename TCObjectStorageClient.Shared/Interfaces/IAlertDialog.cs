using System;

namespace TCObjectStorageClient.Interfaces
{
    public interface IAlertDialog
    {
        void ShowAlert(string alertText);

        void ShowAlert(string alertText, Action<bool> callback);
    }
}
