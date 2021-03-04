using System;

namespace RestSample.Core.Exceptions
{
    public class InvalidApiKeysException : Exception
    {
        public InvalidApiKeysException(string message) : base(message)
        {
        }
    }
}
