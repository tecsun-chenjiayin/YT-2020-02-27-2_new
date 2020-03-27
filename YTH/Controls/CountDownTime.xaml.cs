using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YTH.Functions;

namespace YTH.Controls
{
    /// <summary>
    /// CountDownTime.xaml 的交互逻辑
    /// </summary>
    public partial class CountDownTime : UserControl
    {
        const ulong maxTime = 180;
        ulong nowTime = maxTime;
        ThreadProperty timeTP = null;
        public CountDownTime()
        {
            InitializeComponent();
            timeTP = new ThreadProperty(1000, false, false, update, this);
            Visibility = Visibility.Hidden;
        }

        //开始并显示
        public void start()
        {
            nowTime = maxTime;
            time.Text = nowTime.ToString("D3") + "s";
            timeTP.start();
            Visibility = Visibility.Visible;
        }
        //结束并隐藏
        public void stop()
        {
            timeTP.stop();
            Visibility = Visibility.Hidden;
            time.Text = "180s";
        }
        //重置时间
        public void resetTime()
        {
            nowTime = maxTime;
            time.Text = nowTime.ToString("D3") + "s";
        }
        //计数
        private void update()
        {
            nowTime--;
            if (nowTime == 0)
            {
                stop();
                BackExit.Exit();
            }
            else
            {
                time.Text = nowTime.ToString("D3") + "s";
            }
        }

    }
}
