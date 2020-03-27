using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YTH.Functions
{
    //线程管理所用的状态类
    class StatusData
    {
        public static Action exit = null;
        ///##################### 长期运行的子线程管理 ###########################
        //为关闭子线程而准备的变量和方法
        private static bool isCloseWindow = false;
        private static int subThreadCount = 0;
        private static object locker = new object();
        //###################### 多线程控制 #####################################
        public static string timeTag = "";
        public static string UpdateTimeTag()
        {
            timeTag = Log.GetTime();
            return timeTag;
        }
        public static bool IsTheSameTag(string timeTag2)
        {
            return timeTag == timeTag2;
        }

        //是否打开了密文密码键盘
        public static bool isOpenKeyBoard = false;//在点击退出或返回按钮的时候，根据这个标记判断是否关闭密码键盘
        //创建新的子线程的时候调用      （长期运行的子线程方法的第一句，一个方法只允许调用一次）
        public static void AddThread()
        {
            lock (locker)
            {
                subThreadCount++;
            }
        }
        //结束子线程的时候调用          （如果某方法调用了addThread()，则在该方法返回前调用一次）
        public static void CloseThread()
        {
            lock (locker)
            {
                subThreadCount--;
                if (isCloseWindow && subThreadCount == 0 && exit != null)
                    exit();
            }
        }
        //在监听关闭窗口的方法里面调用
        public static void WindowAboutClose()
        {
            isCloseWindow = true;
        }
        //判断是否继续执行线程
        public static bool IsKeepRuningThread()
        {
            return isCloseWindow == false;
        }

        ///##################### 返回或退出按钮点击控制 ###########################
        ///<summary>用来标记用户是否点击了返回或退出按钮，使用前须将其设置为false</summary>
        //public static bool isClickBackOrExitBtn = false;
        ///##################### 是否继续读银联卡 ###########################
        public static bool isKeepReadYLCar = false;

    }
}
