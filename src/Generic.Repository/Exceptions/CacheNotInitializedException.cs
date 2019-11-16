using System;

namespace Generic.Repository.Exceptions
{
    public class CacheNotInitializedException : BaseException
    {
        protected new string CustomMessage { get; set; } = "Cache not initialized on {0}";

        /// <summary>Initializes a new instance of the <see cref="CacheNotInitializedException"/> class.</summary>
        public CacheNotInitializedException()
        {

        }

        /// <summary>Initializes a new instance of the <see cref="CacheNotInitializedException"/> class.</summary>
        /// <param name="type">The type.</param>
        public CacheNotInitializedException(string type)
        : base(GetMessage(type))
        {

        }

        /// <summary>Initializes a new instance of the <see cref="CacheNotInitializedException"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="inner">The inner.</param>
        public CacheNotInitializedException(string type, Exception inner)
        : base(GetMessage(type), inner)
        {

        }

    }
}
