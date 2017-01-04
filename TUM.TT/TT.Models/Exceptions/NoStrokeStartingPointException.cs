using System;
using System.Runtime.Serialization;

namespace TT.Models.Exceptions
{
    [Serializable]
    public class NoStrokeStartingPointException : Exception
    {
        public NoStrokeStartingPointException()
        {
        }

        public NoStrokeStartingPointException(string message) : base(message)
        {
        }

        public NoStrokeStartingPointException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoStrokeStartingPointException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}