using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultipleThreadsSource
{
    class Program
    {
        private static Threads threads = new Threads();

        static void Main(string[] args)
        {
            //threads.GetCurrentThreadInfo();

            //threads.MainThread();

            //new AccountSample().ShowSample();

            //new MonitorSample().ShowSample();

            //new ThreadPoolSample().ShowSample();

            //new TimerSample().ShowSample();

            //new MutexSample().ShowSample();

            new InvokeSample().ShowSample();

            Console.ReadLine();
        }
    }
}
