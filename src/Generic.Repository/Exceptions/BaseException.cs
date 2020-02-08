using System;

namespace Generic.Repository.Exceptions
{
    public abstract class BaseException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="BaseException"/> class.</summary>
        protected BaseException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="BaseException"/> class.</summary>
        /// <param name="message">The message.</param>
        protected BaseException(string message)
        : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="BaseException"/> class.</summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        protected BaseException(string message, Exception inner)
        : base(message, inner)
        {
        }

        /// <summary>Gets the message.</summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        protected static string GetMessage(string message, params object[] type) => string.Format(message, type);
    }
}