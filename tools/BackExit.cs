using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WH_NEW
{
    /// <summary>
    /// 返回和退出功能封装
    /// </summary>
    class BackExit
    {
        static bool nowIsBack = false;
        static Action back = null;
        public static Action next = null;
        static List<Action> backs = new List<Action>();
        static List<Action> whenBackOrExit = new List<Action>();
        static bool isCanBeReplace = false;
        static bool isInMain2 = false;

        //在首页的时候也要调用一次setBack
        public static void setBack(Action back_)
        {
            if (nowIsBack == false && back != null && isCanBeReplace == false)
                backs.Add(back);
            back = back_;
            nowIsBack = false;
            isCanBeReplace = false;
            if (backs.Count > 1)
                isInMain2 = false;
        }
        public static void setBack_WitchCanBeReplace(Action back_)
        {
            setBack(back_);
            isCanBeReplace = true;
        }
        public static void AddActionWhenBackOrExit(Action a)
        {
            return;
            whenBackOrExit.Add(a);
        }
        public static void ClearAWB()
        {
            whenBackOrExit.Clear();
        }
        //返回
        public static void Back()
        {
            if(backs.Count > 0)
            {
                nowIsBack = true;
                if (backs[backs.Count - 1] != null)
                {
                    next = backs[backs.Count - 1];
                }
                backs.RemoveAt(backs.Count - 1);

                if (whenBackOrExit.Count != 0)
                {
                    foreach (Action action in whenBackOrExit)
                        action();
                    whenBackOrExit.Clear();
                }
                else
                {
                    next();
                }
                
            }
        }
        //返回主页
        public static void Exit()
        {
            isInMain2 = true;
            if (backs.Count >0)
            {
                nowIsBack = true;
                next = backs[0];
                backs.Clear();

                if (whenBackOrExit.Count != 0)
                {
                    
                    foreach (Action action in whenBackOrExit)
                        action();
                    whenBackOrExit.Clear();
                   
                }
                else
                {
                    next();
                }
            }
        }

        //让下次点击返回或退出按钮都响应退出事件
        public static void LetNextClickToMain()
        {
            Action a = backs[0];
            backs.Clear();
            backs.Add(a);
        }

        public static bool isInMain()
        {
            return isInMain2;
        }

    }
}
