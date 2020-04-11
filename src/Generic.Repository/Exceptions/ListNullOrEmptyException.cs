using System;

namespace Generic.Repository.Exceptions
{
    public class ListNullOrEmptyException : BaseException
    {
        /// <summary>Initializes a new instance of the <see cref="ListNullOrEmptyException"/> class.</summary>
        public ListNullOrEmptyException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ListNullOrEmptyException"/> class.</summary>
        /// <param name="type">The type.</param>
        public ListNullOrEmptyException(string type)
        : base(GetMessage(CustomMessage, type, string.Empty))
        {
        }

        public ListNullOrEmptyException(string type, string methodName)
        : base(GetMessage(CustomMessage, type, methodName))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ListNullOrEmptyException"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="inner">The inner.</param>
        public ListNullOrEmptyException(string type, Exception inner)
        : base(GetMessage(CustomMessage, type, string.Empty), inner)
        {
        }

        private static string CustomMessage { get; set; } =
            "The parameter {0} is null or empty. \n Method > {1}.";
    }
}