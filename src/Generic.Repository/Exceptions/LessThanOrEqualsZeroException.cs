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
        /// <param name="type">The type.</param>
        public LessThanOrEqualsZeroException(string type)
        : base(GetMessage(CustomMessage, type))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="LessThanOrEqualsZeroException"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="inner">The inner.</param>
        public LessThanOrEqualsZeroException(string type, Exception inner)
        : base(GetMessage(CustomMessage, type), inner)
        {
        }

        private static string CustomMessage { get; set; } = "The value {0} is less than zero.";
    }
}