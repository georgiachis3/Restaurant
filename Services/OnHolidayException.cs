using System;
using System.Runtime.Serialization;

namespace Web.Models
{
    [Serializable]
    internal class OnHolidayException : Exception
    {
        public OnHolidayException()
        {
        }

        public OnHolidayException(string message) : base(message)
        {
        }

        public OnHolidayException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OnHolidayException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}