using System;
using System.Runtime.Serialization;

namespace TT.Models.Exceptions
{
    [Serializable]
    public class NoStrokeEndPointException : Exception
    {
        public NoStrokeEndPointException()
        {
        }

        public NoStrokeEndPointException(string message) : base(message)
        {
        }

        public NoStrokeEndPointException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoStrokeEndPointException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}