using System;

namespace RestSample.Core.Exceptions
{
    public class FailedToConnectException : Exception
    {
        public FailedToConnectException(string message) : base(message)
        {
        }
    }
}
