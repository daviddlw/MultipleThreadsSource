using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultipleThreadsSource
{
    public class MonitorSample
    {
        public int MonitorSampleTest()
        {
            Cell cell = new Cell();
            int result = 0;

            CellProc producer = new CellProc(cell, 20);
            CellCons consumer = new CellCons(cell, 20);

            Thread threadProc = new Thread(new ThreadStart(producer.ThreadRun));
            threadProc.Name = "Produce Thread";
            Thread threadCons = new Thread(new ThreadStart(consumer.ThreadRun));
            threadCons.Name = "Consume THread";

            try
            {
                threadProc.Start();
                threadCons.Start();

                threadProc.Join();
                threadCons.Join();
            }
            catch (ThreadStateException ex)
            {
                Console.WriteLine(ex.Message);
                result = 1;
            }
            catch (ThreadInterruptedException ex)
            {
                Console.WriteLine(ex.Message);
                result = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
            
        }
    }

    /// <summary>
    /// Cell元素
    /// </summary>
    public class Cell
    {
        int cellContents = 0;
        bool readFlag = false; //true为正在读取，false为正在写入。

        /// <summary>
        /// 从cell中读取内容
        /// </summary>
        /// <returns></returns>
        public int ReadFromCell()
        {
            lock (this)
            {
                if (!readFlag)
                {
                    try
                    {
                        Monitor.Wait(this); //等到Write的地方唤醒Pulse
                    }
                    catch (SynchronizationLockException ex)
                    {
                        Console.WriteLine("A synchronizationLockException has been throwed!, message info：{0}", ex.Message);
                    }
                    catch (ThreadInterruptedException ex)
                    {
                        Console.WriteLine("A threadInterruptedException has been throwed!, message info：{0}", ex.Message);
                    }
                }

                Console.WriteLine("Consume cell contents：{0}", cellContents);
                readFlag = false;
                Monitor.Pulse(this);
            }

            return cellContents;
        }

        /// <summary>
        /// 从cell中写入内容
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public void WriteToCell(int n)
        {
            lock (this)
            {
                if (readFlag)
                {
                    try
                    {
                        Monitor.Wait(this); //等到Read的地方唤醒Pulse
                    }
                    catch (SynchronizationLockException ex)
                    {
                        Console.WriteLine("A synchronizationLockException has been throwed!, message info：{0}", ex.Message);
                    }
                    catch (ThreadInterruptedException ex)
                    {
                        Console.WriteLine("A threadInterruptedException has been throwed!, message info：{0}", ex.Message);
                    }
                }

                cellContents = n;
                Console.WriteLine("Produce cell contents：{0}", cellContents);
                readFlag = true;
                Monitor.Pulse(this);
            }
        }
    }

    public class CellProc
    {
        Cell cell;
        int initialNum = 0;

        public CellProc(Cell sourceCell, int num)
        {
            cell = sourceCell;
            initialNum = num;
        }

        public void ThreadRun()
        {
            for (int i = 0; i < initialNum; i++)
            {
                cell.WriteToCell(i);
            }
        }
    }

    public class CellCons
    {
        Cell cell;
        int initialNum = 0;

        public CellCons(Cell sourceCell, int num)
        {
            cell = sourceCell;
            initialNum = num;
        }

        public void ThreadRun()
        {
            for (int i = 0; i < initialNum; i++)
            {
                cell.ReadFromCell();
            }
        }
    }
}
