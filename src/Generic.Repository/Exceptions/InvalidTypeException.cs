using System;

namespace Generic.Repository.Exceptions
{
    /// <summary>
    /// Invalid type exception.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class InvalidTypeException : BaseException
    {
        protected new string CustomMessage { get; set; } = "The type {0} is invalid for this operation.";

        /// <summary>Initializes a new instance of the <see cref="InvalidTypeException"/> class.</summary>
        public InvalidTypeException()
        {

        }

        /// <summary>Initializes a new instance of the <see cref="InvalidTypeException"/> class.</summary>
        /// <param name="type">The type.</param>
        public InvalidTypeException(string type)
        : base(GetMessage(type))
        {

        }

        /// <summary>Initializes a new instance of the <see cref="InvalidTypeException"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="inner">The inner.</param>
        public InvalidTypeException(string type, Exception inner)
        : base(GetMessage(type), inner)
        {

        }
    }
}
