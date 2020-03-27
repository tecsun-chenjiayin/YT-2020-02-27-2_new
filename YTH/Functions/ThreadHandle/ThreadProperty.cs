using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace YTH.Functions
{
    /// <summary>
    /// 常驻线程使用的信息
    /// </summary>
    public class ThreadProperty
    {
        //除了ForeverThread，外部不应该操作下面的成员
        public ulong n = 0;//时间间隔 = n * 50
        public ulong N = 1;//实际计时
        public bool isOnce = false;//是不是只执行一次
        public bool isEnable = true;//当前是不是可用
        public Action action = null;//执行的内容
        public UIElement srcUI = null;//源UI对象，可以为空，如果为耗时操作，则应该置为空
        public ThreadProperty(ulong time, bool isOnce, bool isEnable, Action action, UIElement srcUI)
        {
            n = time / 50;
            if (n < 1) n = 1;
            this.isOnce = isOnce;
            this.isEnable = isEnable;
            this.action = action;
            this.srcUI = srcUI;
            TH.add(this);
        }
        //开始
        public void start()
        {
            N = 1;
            isEnable = true;
            if (srcUI == null)
                TH.add(this);
        }
        //下一个循环的时候不再执行
        public void stop()
        {
            N = 1;
            isEnable = false;
        }
        public void resetTime(ulong time)
        {
            n = time / 50;
        }

        public bool hasStart()
        {
            return isEnable;
        }
    }
}
