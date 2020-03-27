using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace YTH.Functions
{
   
    /// <summary>
    /// 常驻线程
    /// </summary>
    public class ForeverThread
    {
        List<ThreadProperty> values = new List<ThreadProperty>();
        ThreadProperty[] aps = new ThreadProperty[1000];
        object locker = new object();
        ulong num = 1;
        public bool isWorking = false;//是否正在处理数据
        public ForeverThread()
        {
            Thread t = new Thread(new ThreadStart(FT));
            t.Start();
        }

        public void addAction(ThreadProperty ap)
        {
            lock (locker)
            {
                if (values.Contains(ap) == false)
                    values.Add(ap);
            }
        }
        public void removeAction(ThreadProperty ap)
        {
            lock (locker)
            {
                if (values.Contains(ap))
                    values.Remove(ap);
            }
        }
        public bool Contains(ThreadProperty ap)
        {
            return values.Contains(ap);
        }

        private void FT()
        {
            StatusData.AddThread();
            Thread.Sleep(2000);
            while (StatusData.IsKeepRuningThread())
            {
                lock (locker)
                {
                    if (aps.Length < values.Count)
                        aps = new ThreadProperty[aps.Length + 1000];
                    int index = 0;
                    foreach (ThreadProperty val in values)
                        aps[index++] = val;
                    if (index < aps.Length)
                        aps[index] = null;
                }
                for (int i = 0; i < aps.Length; i++)
                {
                    if (aps[i] == null) continue;
                    if (aps[i].isEnable && aps[i].action != null && aps[i].N % aps[i].n == 0)
                    {
                        if (aps[i].isOnce)
                        {
                            aps[i].isEnable = false;
                            aps[i].N = 1;
                        }
                        isWorking = true;
                        if(aps[i].srcUI == null)
                        {
                            try
                            {
                                aps[i].action();
                                //数据处理线程只运行一次,再次运行需重写添加以分配空闲线程处理数据
                                if (aps[i].isOnce)
                                    removeAction(aps[i]);
                            }
                            catch(Exception e)
                            {
                                Log.AddLog("ForeverThread_Data", e.ToString());
                            }
                        }
                        else
                        {
                            try
                            {
                                aps[i].srcUI.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (aps[i].action));
                            }
                            catch (Exception e)
                            {
                                Log.AddLog("ForeverThread_UI", e.ToString());
                            }
                        }   
                        isWorking = false;
                    }
                    if (aps[i].isEnable)
                        aps[i].N++;
                }
                Thread.Sleep(50);
                num++;
            }
            StatusData.CloseThread();
        }
    }

}
