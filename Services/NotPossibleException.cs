﻿using System;
using System.Runtime.Serialization;

namespace Web.Models
{
    [Serializable]
    internal class NotPossibleException : Exception
    {
        public NotPossibleException()
        {
        }

        public NotPossibleException(string message) : base(message)
        {
        }

        public NotPossibleException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotPossibleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}