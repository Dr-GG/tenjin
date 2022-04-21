namespace Tenjin.Tests.Services
{
    public class ThreadCounterMonitor
    {
        private readonly object _root = new();

        public int Counter { get; private set; }

        public void Increment()
        {
            lock (_root)
            {
                Counter++;
            }
        }
    }
}
