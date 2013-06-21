using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;

namespace MultipleThreadsSource
{
    /// <summary>
    /// 利用BeginInvoke与EndInvoke制作的猜数字小游戏
    /// </summary>
    public class InvokeSample
    {
        private bool isCorrect = false;
        private delegate void MessageHandler(int num);
        private Dictionary<bool, string> dict = new Dictionary<bool, string>();
        private int storedNum = 0;

        public InvokeSample()
        {
            dict.Add(true, "等待中...");
            dict.Add(false, "等待中......");
        }

        /// <summary>
        /// 猜测100以内的数字
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public void GuessNumber(int num)
        {
            Random rd = new Random();            
            int currentNum = (storedNum != 0 && !isCorrect) ? storedNum : rd.Next(100);
            Thread.Sleep(2000);
            if (currentNum == num)
            {
                Console.Clear();
                Console.WriteLine("恭喜你，猜对了数字为：" + num);
                isCorrect = true;
                storedNum = 0;
            }
            else if (currentNum <= num)
            {
                Console.Clear();
                Console.WriteLine("你输入的数值{0}，高了！", num);
                storedNum = currentNum;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("你输入的数值{0}，低了！", num);
                storedNum = currentNum;
            }
        }

        /// <summary>
        /// 显示Sample
        /// </summary>
        public void ShowSample()
        {
            MessageHandler taskHandler = new MessageHandler(GuessNumber);
            IAsyncResult asyncResult = null;
            do
            {
                Console.WriteLine("请输入猜测的数值：");
                string consoleStr = Console.ReadLine();
                Regex regx = new Regex(@"^\d+$");

                if (!regx.IsMatch(consoleStr))
                {
                    Console.WriteLine("格式不正确，请输入数值型。");
                    continue;
                }

                int guessNum = int.Parse(consoleStr);

                asyncResult = taskHandler.BeginInvoke(guessNum, null, null);

                bool flag = true;
                while (!asyncResult.IsCompleted)
                {
                    Console.Clear();
                    Console.WriteLine(dict[flag]);
                    Thread.Sleep(1000);
                    flag = !flag;
                }

            } while (!isCorrect);

            taskHandler.EndInvoke(asyncResult);
        }
    }
}
