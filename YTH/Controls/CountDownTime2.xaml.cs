using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using YTH.Functions;

namespace YTH.Controls
{
    /// <summary>
    /// CountDownTime2.xaml 的交互逻辑
    /// </summary>
    public partial class CountDownTime2 : UserControl
    {
        bool isStop = false;
        static int nowTime = 0;
        static int maxTime = 0;
        string timeTag = null;
        public Action whenExit = null;

        public static void updateTime()
        {
            if(maxTime > 0)
                nowTime = maxTime;
        }

        public CountDownTime2()
        {
            InitializeComponent();
        }

        public void start(string name = null, int maxTime_ = 180)
        {
            Visibility = Visibility.Visible;
            timeTag = CD.timeTag.updateTag();
            isStop = false;
            nowTime = maxTime_;
            maxTime = maxTime_;
            if (name == null)
            {
                this.name.Visibility = Visibility.Hidden;
                nameTip.Visibility = Visibility.Hidden;
            }
            else
            {
                this.name.Text = name;
                this.name.Visibility = Visibility.Visible;
                nameTip.Visibility = Visibility.Visible;
            }

            Thread thread = new Thread(new ThreadStart(handle));
            thread.Start();
        }

        public void stop()
        {
            isStop = true;
            Visibility = Visibility.Hidden;
        }

        private void handle()
        {
            string timeTag2 = timeTag;
            while (isStop == false && nowTime >= 0 && timeTag == timeTag2)
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(updateUI));
                nowTime--;
                Thread.Sleep(1000);
            }

            if (isStop == false && nowTime < 0 && timeTag == timeTag2)
            {
                if(whenExit != null)
                    TH.addOnceUI(whenExit);
                TH.addOnceUI(BackExit.Exit);
            }
        }

        private void updateUI()
        {
            time.Text = nowTime.ToString();
        }
    }
}
