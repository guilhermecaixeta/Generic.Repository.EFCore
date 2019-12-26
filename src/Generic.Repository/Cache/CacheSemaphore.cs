using System.Threading;

namespace Generic.Repository.Cache
{
    internal static class CacheSemaphore
    {
        public static Semaphore Semaphore;

        public static bool Signal = true;

        private const int MaxValue = 1;

        private const int MinValue = 1;

        public static void InitializeSemaphore()
        {
            Semaphore = new Semaphore(MinValue, MaxValue, "CacheAccess");
        }

        public static void Release()
        {
            Semaphore.Release(MaxValue);
        }

        public static void WaitOne()
        {
            Signal = Semaphore.WaitOne(5);
        }
    }
}