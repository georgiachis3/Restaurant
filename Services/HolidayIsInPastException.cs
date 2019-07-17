using System;
using System.Runtime.Serialization;

namespace Web.Data
{
    [Serializable]
    internal class HolidayIsInPastException : Exception
    {
        public HolidayIsInPastException()
        {
        }

        public HolidayIsInPastException(string message) : base(message)
        {
        }

        public HolidayIsInPastException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HolidayIsInPastException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}