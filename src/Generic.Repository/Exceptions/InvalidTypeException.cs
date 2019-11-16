using System;

namespace Generic.Repository.Validations.Exceptions
{
    /// <summary>
    /// Invalid type exception.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class InvalidTypeException : Exception
    {
        private new const string Message = "The type {0} is invalid for this operation.";

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

        /// <summary>Gets the message.</summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static string GetMessage(string type) => string.Format(Message, type);
    }
}
