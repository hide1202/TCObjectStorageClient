using System;

namespace ToastCloud.ObjectStorage.Exceptions
{
    public class ObjectStorageSdkException : Exception
    {
        public ObjectStorageSdkException(string message) : base(message)
        {
        }
    }
}
