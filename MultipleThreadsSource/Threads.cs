using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultipleThreadsSource
{
    public class Threads
    {
        private const string start = "------Begin------";
        private const string end = "------Begin------";

        /// <summary>
        /// 获取当前线程信息
        /// </summary>
        public void GetCurrentThreadInfo()
        {
            Console.WriteLine(start);
            Thread.CurrentThread.Name = "System Thread";
            Console.WriteLine("当前线程名字：{0}，线程状态：{1}", Thread.CurrentThread.Name, Thread.CurrentThread.ThreadState.ToString());
            Console.WriteLine(end);
        }

        /// <summary>
        /// 子线程
        /// </summary>
        public void SubThread()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("This is thread {0}{1}", Thread.CurrentThread.Name, i);
            }
        }

        public void MainThread()
        {
            Console.WriteLine("Main Thread Start!");
            Thread.CurrentThread.Name = "Main Thread";
            Thread oThread = new Thread(new ThreadStart(SubThread));
            oThread.Name = "Sub Thread";
            for (int i = 0; i < 10; i++)
            {
                if (i == 4)
                {
                    oThread.Start();
                    oThread.Join();
                    Thread currentThread = Thread.CurrentThread;
                }
                else
                {
                    Console.WriteLine("This is {0}{1}", Thread.CurrentThread.Name, i);
                }
            }
        }
    }
}
