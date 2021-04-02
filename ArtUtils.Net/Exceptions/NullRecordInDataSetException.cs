using System;

namespace ArtUtils.Net.Exceptions
{

    public class NullRecordInDataSetException : Exception
    {
        public NullRecordInDataSetException()
        {
        }

        public NullRecordInDataSetException(string message)
            : base(message)
        {
        }

        public NullRecordInDataSetException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
