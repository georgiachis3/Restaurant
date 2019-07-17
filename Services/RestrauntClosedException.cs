using System;
using System.Runtime.Serialization;

namespace Web.Data
{
    [Serializable]
    internal class RestrauntClosedException : Exception
    {
        public RestrauntClosedException()
        {
        }

        public RestrauntClosedException(string message) : base(message)
        {
        }

        public RestrauntClosedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RestrauntClosedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}