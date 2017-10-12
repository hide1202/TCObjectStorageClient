using System;

namespace ToastCloud.ObjectStorage.Exceptions
{
    public class GenericRequestException : Exception
    {
        public GenericRequestException(string message) : base(message)
        {
        }
    }
}
