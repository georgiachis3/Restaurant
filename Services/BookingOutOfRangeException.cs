using System;
using System.Runtime.Serialization;

namespace Web.Data
{
    [Serializable]
    internal class BookingOutOfRangeException : Exception
    {
        public BookingOutOfRangeException()
        {
        }

        public BookingOutOfRangeException(string message) : base(message)
        {
        }

        public BookingOutOfRangeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BookingOutOfRangeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}