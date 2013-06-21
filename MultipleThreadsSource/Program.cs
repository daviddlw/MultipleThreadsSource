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

            //new AccountSample().ShowAccountSample();

            //new MonitorSample().ShowMonitorSample();

            //new ThreadPoolSample().ShowSample();

            //new TimerSample().ShowSample();

            new MutexSample().ShowSample();

            Console.ReadLine();
        }
    }
}
