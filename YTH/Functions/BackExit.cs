using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YTH.Functions
{
    /// <summary>
    /// 返回和退出功能封装
    /// </summary>
    class BackExit
    {
        static bool nowIsBack = false;
        static Action back = null;
        static List<Action> backs = new List<Action>();

        public static void setBack(Action back_)
        {
            if (nowIsBack == false && back != null)
                backs.Add(back);
            back = back_;
            nowIsBack = false;
        }  
        //返回
        public static void Back()
        {
            if(backs.Count > 0)
            {
                nowIsBack = true;
                backs[backs.Count - 1]();
                backs.RemoveAt(backs.Count - 1);
                CD.timeTag.updateTag(); 
            }
            if(backs.Count == 0)
            {
                Exit();
                //CD.countDownTime.stop();
            }
                
        }
        //返回主页
        public static void Exit()
        {
            if (backs.Count == 0 && back != null)
                backs.Add(back);
            if(backs.Count >0)
            {
                nowIsBack = true;
                backs[0]();
                backs.Clear();
                if(CD.countDownTime != null)
                    CD.countDownTime.stop();
                if(CD.timeTag != null)
                    CD.timeTag.updateTag();
                if (CD.business2 != null)
                    CD.business2.stop();
                if (CD.business1 != null)
                    CD.business1.stop();
                CD.setTopUI(null);
            }
        }

        public static bool isOnMain()
        {
            return backs.Count == 0;
        }

        //让下次点击返回或退出按钮都响应退出事件
        public static void LetNextClickToMain()
        {
            if (backs.Count > 0)
            {
                Action a = backs[0];
                backs.Clear();
                backs.Add(a);
            }
        }


    }
}
