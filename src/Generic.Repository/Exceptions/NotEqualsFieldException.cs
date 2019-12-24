using System;

namespace Generic.Repository.Exceptions
{
    public class NotEqualsFieldException : BaseException
    {
        /// <summary>Initializes a new instance of the <see cref="NotEqualsFieldException"/> class.</summary>
        public NotEqualsFieldException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="NotEqualsFieldException"/> class.</summary>
        /// <param name="type1">The type1.</param>
        /// <param name="type2">The type2.</param>
        public NotEqualsFieldException(string type1, string type2)
        : base(GetMessage(CustomMessage, type1, type2))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="NotEqualsFieldException"/> class.</summary>
        /// <param name="type1">The type1.</param>
        /// <param name="type2">The type2.</param>
        /// <param name="inner">The inner.</param>
        public NotEqualsFieldException(string type1, string type2, Exception inner)
        : base(GetMessage(CustomMessage, type1, type2), inner)
        {
        }

        private static string CustomMessage { get; set; } = "Fields are not equals: {0} != {1}";
    }
}