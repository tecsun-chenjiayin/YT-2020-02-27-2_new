using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace YTH.Functions
{
    class TH
    {
        const int subThreadNum = 2;//常驻子线程数量，最少为2，第1个是处理UI的线程，其余是处理数据的线程
        static List<ForeverThread> fts = new List<ForeverThread>();
        private static void init()
        {
        
            for (int i = 0; i < subThreadNum; i++)
                fts.Add(new ForeverThread());
        }

        private static void addUIHandle(ThreadProperty tp)
        {
            if (fts.Count == 0)
                init();
            fts[0].addAction(tp);
        }

        private static void addDataHandle(ThreadProperty tp)
        {
            if (fts.Count == 0)
                init();
            for(int i = 1; i < fts.Count; i++)
            {
                if (fts[i].Contains(tp))
                    return;
            }
            bool hasAdd = false;
            for(int i = 1; i < fts.Count; i++)
            {
                if(fts[i].isWorking == false)
                {
                    hasAdd = true;
                    fts[i].addAction(tp);
                    break;
                }
            }
            if (hasAdd == false)
                fts[1].addAction(tp);
        }

        public static void add(ThreadProperty tp)
        {
            if (tp.srcUI == null)
                addDataHandle(tp);
            else
                addUIHandle(tp);
        }

        static ThreadProperty keepUI = null;
        static Queue<Action> actions = new Queue<Action>();
        public static UIElement mainUI = null;//长期存在的ui对象
        public static void addOnceUI(Action step)
        {
            //if (keepUI == null)
            //    keepUI = new ThreadProperty(50, false, false, uiaction, mainUI);
            //actions.Enqueue(step);
            //keepUI.start();
            if(step != null)
                mainUI.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (step));
        }
        public static void uiaction()
        {
            if (actions.Count != 0)
            {
                Action action = actions.Dequeue();
                if (action != null)
                    action();
            }
        }

        static ThreadProperty keepData = null;
        static Queue<Action> actions2 = new Queue<Action>();
        public static void addOnceData(Action action)
        {
            //if (keepData == null)
                //keepData = new ThreadProperty(50, false, false, dataaction, null);
            //actions2.Enqueue(action);
            //keepData.start();

            Thread t = new Thread(new ThreadStart(action));
            t.Start();
        }
        public static void dataaction()
        {
            if (actions2.Count != 0)
            {
                Action action = actions2.Dequeue();
                if (action != null)
                    action();
            }
        }
    }
}
