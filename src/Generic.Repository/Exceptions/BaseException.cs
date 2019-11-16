using System;
using System.Collections.Generic;
using System.Text;

namespace Generic.Repository.Exceptions
{
    public abstract class BaseException : Exception
    {
        protected static string CustomMessage { get; set; } = "The custom message {0}.";

        /// <summary>Initializes a new instance of the <see cref="BaseException"/> class.</summary>
        public BaseException()
        {

        }

        /// <summary>Initializes a new instance of the <see cref="BaseException"/> class.</summary>
        /// <param name="type">The type.</param>
        public BaseException(string type)
        : base(GetMessage(type))
        {

        }

        /// <summary>Initializes a new instance of the <see cref="BaseException"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="inner">The inner.</param>
        public BaseException(string type, Exception inner)
        : base(GetMessage(type), inner)
        {

        }

        /// <summary>Gets the message.</summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        protected static string GetMessage(params string[] type) => string.Format(CustomMessage, type);
    }
}

