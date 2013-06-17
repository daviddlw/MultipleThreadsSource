using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace MultipleThreadsSource
{
    public class ThreadPoolSample
    {
        Stopwatch progWatch = new Stopwatch();
        IList<object> objLs = new List<object>();
        int total = 0;
        public void ShowSample()
        {
            //Simple();
            //Add1000000VarObj();
            //Add1000000VarObjByMultipleThreads();
            Add1000000VarObjByMultipleThreadsNew();
        }

        private void Simple()
        {
            WaitCallback waitCallback = new WaitCallback(ShowMessage);

            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(waitCallback, i + 1);
            }
        }

        private void ShowMessage(object state)
        {
            string message = state == null ? string.Empty : state.ToString();
            Console.WriteLine("线程{0}运行启动！", message);
            Thread.Sleep(2000);
            Console.WriteLine("线程{0}运行结束！", message);
        }

        private void Calculate1To1000000Total()
        {
            for (int i = 1; i <= 2500000; i++)
            {
                object obj = new { id = i, name = string.Format("name{0}", i) };
                objLs.Add(obj);
            }
            Console.WriteLine("添加{0}个匿名对象", objLs.Count.ToString("n0"));
        }

        private void Add1000000VarObj()
        {
            progWatch.Start();
            Calculate1To1000000Total();
            progWatch.Stop();
            Console.WriteLine("添加{0}个匿名对象总共花费了{1}毫秒", objLs.Count.ToString("n0"), progWatch.ElapsedMilliseconds);
        }

        private void Add1000000VarObjByMultipleThreads()
        {
            progWatch.Start();
            WaitCallback callback = new WaitCallback(AddObj);
            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(callback, 100000);
            }
            progWatch.Stop();
            Console.WriteLine("添加1,000,000个匿名对象总共花费了{0}毫秒", progWatch.ElapsedMilliseconds);
        }

        private void AddObj(object maxCount)
        {
            int count = maxCount == null ? 0 : int.Parse(maxCount.ToString());
            for (int i = 1; i <= count; i++)
            {
                object obj = new { id = i, name = string.Format("name{0}", i) };
                objLs.Add(obj);
            }
            total += objLs.Count;
            Console.WriteLine("此次添加了{0}个匿名对象，共有{1}个匿名对象", objLs.Count, total);
        }

        private void AddObj()
        {
            lock (objLs)
            {
                try
                {
                    for (int i = 1; i <= 500000; i++)
                    {
                        object obj = new { id = i, name = string.Format("name{0}", i) };
                        objLs.Add(obj);
                    }
                    total += objLs.Count;
                }
                catch (SynchronizationLockException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine("此次添加了{0}个匿名对象，共有{1}个匿名对象", 100000, objLs.Count);
        }


        private void Add1000000VarObjByMultipleThreadsNew()
        {
            IList<Thread> threads = new List<Thread>();
            progWatch.Start();
            for (int i = 0; i < 5; i++)
            {
                Thread thread = new Thread(new ThreadStart(AddObj));
                thread.Start();
                thread.Join();
            }

            progWatch.Stop();
            Console.WriteLine("添加1,000,000个匿名对象总共花费了{0}毫秒", progWatch.ElapsedMilliseconds);
        }
    }
}
