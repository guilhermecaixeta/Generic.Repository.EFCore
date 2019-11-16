using System;

namespace Generic.Repository.Exceptions
{
    public class NotEqualsFieldException : BaseException
    {

        protected new string CustomMessage { get; set; } = "Fields are not equals {0} != {1}";

        /// <summary>Initializes a new instance of the <see cref="NotEqualsFieldException"/> class.</summary>
        public NotEqualsFieldException()
        {

        }

        /// <summary>Initializes a new instance of the <see cref="NotEqualsFieldException"/> class.</summary>
        /// <param name="type">The type.</param>
        public NotEqualsFieldException(string type1, string type2)
        : base(GetMessage(type1, type2))
        {

        }

        /// <summary>Initializes a new instance of the <see cref="NotEqualsFieldException"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="inner">The inner.</param>
        public NotEqualsFieldException(string type1, string type2, Exception inner)
        : base(GetMessage(type1, type2), inner)
        {

        }
    }
}
