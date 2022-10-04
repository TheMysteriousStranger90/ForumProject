using System;
using System.Runtime.Serialization;

namespace BLL.Exceptions
{
    [Serializable]
    public class ForumProjectException : Exception
    {
        private static readonly string DefaultMessage = "Forum Project exception was thrown.";

        public ForumProjectException() : base(DefaultMessage)
        {
        }

        public ForumProjectException(string message) : base(message)
        {
        }

        public ForumProjectException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ForumProjectException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}