using System;
using System.Runtime.Serialization;

namespace Web.Data
{
    [Serializable]
    internal class GreaterThanMonthException : Exception
    {
        public GreaterThanMonthException()
        {
        }

        public GreaterThanMonthException(string message) : base(message)
        {
        }

        public GreaterThanMonthException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GreaterThanMonthException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}