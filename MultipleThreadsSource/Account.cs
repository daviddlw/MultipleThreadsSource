using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultipleThreadsSource
{
    /// <summary>
    /// 账户信息
    /// </summary>
    public class Account
    {
        private int balance = 0;

        /// <summary>
        /// 初始化账户
        /// </summary>
        /// <param name="account"></param>
        public Account(int account)
        {
            this.balance = account;
        }

        /// <summary>
        /// 账户交易
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        private int WithDraw(int account)
        {
            if (balance < 0)
                //throw new Exception("Balance is smaller than zero!");
                Console.WriteLine("Balance is smaller than zero!");

            lock (this)
            {
                Console.WriteLine("Current Thread Name：{0}", Thread.CurrentThread.Name);
                if (balance >= account)
                {
                    Thread.Sleep(5);
                    balance = balance - account;
                    return account;
                }
                else
                {
                    return 0;
                }
            }
        }

        public void DoTransaction()
        {
            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Your total account is {0}", balance);
                Console.WriteLine("Current account:{0}", WithDraw(r.Next(-50, 100)));
                Console.WriteLine("Your account balance is {0}", balance);
                Console.WriteLine("-----------------分割线---------------");
            }
        }
    }

    public class AccountSample
    {
        public void AccountSampleTest()
        {
            Account account = new Account(0);
            Thread[] threadArr = new Thread[10];
            for (int i = 0; i < 10; i++)
            {
                Thread newThread = new Thread(new ThreadStart(account.DoTransaction));
                newThread.Name = "Thread_" + (i + 1);
                threadArr[i] = newThread;
            }

            for (int i = 0; i < threadArr.Length; i++)
            {
                threadArr[i].Start();
            }
        }
    }
}
