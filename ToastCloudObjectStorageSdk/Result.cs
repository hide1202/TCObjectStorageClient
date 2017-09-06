using System;
using Monad;

namespace ToastCloud.ObjectStorage
{
    public class Result<TSuccess, TFailure>
    {
        public static Result<TSuccess, Exception> FromTry(TryResult<TSuccess> @try)
        {
            return !@try.IsFaulted ?
                Result<TSuccess, Exception>.ToSuccess(@try.Value) :
                Result<TSuccess, Exception>.ToFailure(@try.Exception);
        }

        public static Result<TSuccess, Exception> FromTry<TInput>(TryResult<TInput> @try, Func<TInput, TSuccess> successGenerator)
        {
            return !@try.IsFaulted ?
                Result<TSuccess, Exception>.ToSuccess(successGenerator(@try.Value)) :
                Result<TSuccess, Exception>.ToFailure(@try.Exception);
        }

        public static Result<TSuccess, TFailure> ToSuccess(TSuccess value)
        {
            return new Result<TSuccess, TFailure>() { Value = value };
        }

        public static Result<TSuccess, TFailure> ToFailure(TFailure cause)
        {
            return new Result<TSuccess, TFailure>() { Cause = cause };
        }

        private Result() { }

        public TSuccess Value { get; set; }

        public TFailure Cause { get; set; }

        public bool IsSuccess => Cause == null;
    }
}
