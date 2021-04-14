using System;

namespace ArtUtils.Net.Core.Exceptions
{

    public class TableAttributesException : Exception
    {
        public TableAttributesException()
        {
        }

        public TableAttributesException(string message)
            : base(message)
        {
        }

        public TableAttributesException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
