using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultipleThreadsSource
{
    public class TimerSample
    {
        private ParamObject paramObj;

        public TimerSample()
        {
            paramObj = new ParamObject();
        }

        public void ShowSample()
        {
            Console.WriteLine("定时器启动！Now：{0}", DateTime.Now);
            TimerCallback callback = new TimerCallback(ShowMessage);
            Timer timer = new Timer(callback, paramObj, 0, 2000);
            paramObj.Timer = timer;

        }

        private void ShowMessage(object state)
        {
            ParamObject paramObj = state as ParamObject;
            if (paramObj != null)
            {
                lock (paramObj)
                {
                    if (paramObj.Count <= 10)
                        Console.WriteLine("Hello World! Now：{0}", DateTime.Now);
                    if (paramObj.Count == 5)
                        paramObj.Timer.Change(1000, 1000);
                    if (paramObj.Count > 10)
                    {
                        paramObj.Timer.Dispose();
                        paramObj.Timer = null;
                        Console.WriteLine("定时器销毁！");
                    }

                    paramObj.Count++;
                }
            }
        }
    }

    public class ParamObject
    {
        public ParamObject()
        {
            this.Count = 0;
        }

        public int Count { get; set; }

        public Timer Timer { get; set; }
    }
}
