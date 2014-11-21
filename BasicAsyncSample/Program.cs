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
            // prints out the main thread
            PrintCurrentThreadId();

            DoSomethingAsync().Wait();
        }

        private static async Task DoSomethingAsync()
        {
            // this should print out the id of the thread that is executing Main
            PrintCurrentThreadId();

            // ConfigureAwait(false) - await, but don't have to return back to original synchronization context when done 
            await Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(false);
            
            // diff thread might be executing the next line
            PrintCurrentThreadId();
            await Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(false);

            // diff thread might be executing the next line
            PrintCurrentThreadId();
            Console.WriteLine("Press anykey to exit..");
            Console.ReadKey();
        }

        private static void PrintCurrentThreadId()
        {
            Console.WriteLine("Current thread: {0} ", Thread.CurrentThread.ManagedThreadId);
        }
    }
}
