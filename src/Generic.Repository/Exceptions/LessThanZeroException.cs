using System;

namespace Generic.Repository.Exceptions
{
    public class LessThanZeroException : BaseException
    {
        /// <summary>Initializes a new instance of the <see cref="LessThanZeroException"/> class.</summary>
        public LessThanZeroException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="LessThanZeroException"/> class.</summary>
        /// <param name="type">The type.</param>
        public LessThanZeroException(string type)
        : base(GetMessage(CustomMessage, type))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="LessThanZeroException"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="inner">The inner.</param>
        public LessThanZeroException(string type, Exception inner)
        : base(GetMessage(CustomMessage, type), inner)
        {
        }

        private static string CustomMessage { get; set; } = "The value {0} is less than zero(0).";
    }
}