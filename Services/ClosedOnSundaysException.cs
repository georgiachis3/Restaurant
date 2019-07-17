using System;
using System.Runtime.Serialization;

namespace Web.Data
{
    [Serializable]
    internal class ClosedOnSundaysException : Exception
    {
        public ClosedOnSundaysException()
        {
        }

        public ClosedOnSundaysException(string message) : base(message)
        {
        }

        public ClosedOnSundaysException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClosedOnSundaysException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}