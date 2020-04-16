using System;

namespace Generic.Repository.Exceptions
{
    public class LessThanOrEqualsZeroException : BaseException
    {
        /// <summary>Initializes a new instance of the <see cref="LessThanOrEqualsZeroException"/> class.</summary>
        public LessThanOrEqualsZeroException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="LessThanOrEqualsZeroException"/> class.</summary>
        /// <param name="type">The value.</param>
        public LessThanOrEqualsZeroException(string value)
        : base(GetMessage(CustomMessage, string.Empty, value))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LessThanOrEqualsZeroException"/> class.
        /// </summary>
        /// <param name="nameParameter">The name parameter.</param>
        /// <param name="value">The value.</param>
        public LessThanOrEqualsZeroException(string nameParameter, string value)
        : base(GetMessage(CustomMessage, nameParameter, value))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="LessThanOrEqualsZeroException"/> class.</summary>
        /// <param name="value">The value.</param>
        /// <param name="inner">The inner.</param>
        public LessThanOrEqualsZeroException(string value, Exception inner)
        : base(GetMessage(CustomMessage, string.Empty, value), inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LessThanOrEqualsZeroException"/> class.
        /// </summary>
        /// <param name="nameParameter">The name parameter.</param>
        /// <param name="value">The value.</param>
        /// <param name="inner">The inner.</param>
        public LessThanOrEqualsZeroException(string nameParameter, string value, Exception inner)
        : base(GetMessage(CustomMessage, nameParameter, value), inner)
        {
        }

        /// <summary>
        /// Gets the custom message.
        /// </summary>
        /// <value>
        /// The custom message.
        /// </value>
        private static string CustomMessage { get; } = "The field '{0}' has the value invalid '{1}'!";
    }
}