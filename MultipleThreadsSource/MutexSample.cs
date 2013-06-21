using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultipleThreadsSource
{
    public class MutexSample
    {
        private static Mutex m1;
        private static Mutex m2;
        private static AutoResetEvent event1 = new AutoResetEvent(false);
        private static AutoResetEvent event2 = new AutoResetEvent(false);
        private static AutoResetEvent event3 = new AutoResetEvent(false);
        private static AutoResetEvent event4 = new AutoResetEvent(false);

        public MutexSample()
        {
            m1 = new Mutex(true, "MyMutex");
            m2 = new Mutex(true);
        }

        public void ShowSample()
        {
            Console.WriteLine("Mutex Sample：");
            AutoResetEvent[] resetEvents = new AutoResetEvent[4] { event1, event2, event3, event4 };
            Mutex[] mutexSamplesArray = new Mutex[2] { m1, m2 };
            MutexSample mutexSample = new MutexSample();

            IList<Thread> threadLs = new List<Thread>()
            {
                new Thread(new ThreadStart(t1Start)),
                new Thread(new ThreadStart(t2Start)),
                new Thread(new ThreadStart(t3Start)),
                new Thread(new ThreadStart(t4Start))
            };

            foreach (Thread item in threadLs)
            {
                item.Start();
            }

            Thread.Sleep(2000);
            Console.WriteLine("-Main Thread Release m1");
            m1.ReleaseMutex();

            Thread.Sleep(1000);
            Console.WriteLine("-Main Thread Release m1");
            m2.ReleaseMutex();

            WaitHandle.WaitAll(mutexSamplesArray);
        }

        public void t1Start()
        {
            Console.WriteLine("t1Start started, Mutex.WaitAll(Mutex[])");
            Mutex[] gMutex = new Mutex[2] { m1, m2 };
            Mutex.WaitAll(gMutex);
            Thread.Sleep(1000);
            Console.WriteLine("t1Start has finished!, Mutex.WaitAll(Mutex[]) satisfied!");
            event1.Set();
        }

        public void t2Start()
        {
            Console.WriteLine("t2Start started, m1 wait one!");
            m1.WaitOne();
            Console.WriteLine("t2Start has finished!, m1.WaitOne() satisfied!");
            event2.Set();
        }

        public void t3Start()
        {
            Console.WriteLine("t3Start started, Mutex.WaitAny(Mutex[])");
            Mutex[] gMetux = new Mutex[2] { m1, m2 };
            Mutex.WaitAny(gMetux);
            Thread.Sleep(1000);
            Console.WriteLine("t2Start has finished!, Mutex.WaitAny(Mutex[])");
            event3.Set();
        }

        public void t4Start()
        {
            Console.WriteLine("t4Start started, m2 wait one!");
            m2.WaitOne();
            Console.WriteLine("t2Start has finished!, m2.WaitOne()!");
            event4.Set();
        }
    }
}
