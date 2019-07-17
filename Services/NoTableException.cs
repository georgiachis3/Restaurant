using System;
using System.Runtime.Serialization;

namespace Web.Data
{
    [Serializable]
    internal class NoTableException : Exception
    {
        public NoTableException()
        {
        }

        public NoTableException(string message) : base(message)
        {
        }

        public NoTableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoTableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}