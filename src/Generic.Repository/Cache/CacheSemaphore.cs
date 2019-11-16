using System.Threading;

namespace Generic.Repository.Cache
{
    internal static class CacheSemaphore
    {
        private const int MaxValue = 1;

        public static Semaphore Semaphore;

        public static void InitializeSemaphore()
        {
            Semaphore = new Semaphore(0, MaxValue);
        }

        public static void WaitOne()
        {
            Semaphore.WaitOne();
        }

        public static void Release()
        {
            Semaphore.Release();
        }
    }
}
