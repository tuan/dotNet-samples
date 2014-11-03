using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BasicAsyncSample
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            PrintCurrentThreadId();

            DoSomethingAsync().Wait();
        }

        private static async Task DoSomethingAsync()
        {
            PrintCurrentThreadId();
            
            await Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(false);
            
            PrintCurrentThreadId();
            await Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(false);

            PrintCurrentThreadId();
            Console.ReadKey();
        }

        private static void PrintCurrentThreadId()
        {
            Console.WriteLine("Current thread: {0} ", Thread.CurrentThread.ManagedThreadId);
        }
    }
}
