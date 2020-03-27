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

namespace WpfControlLibrary
{
    /// <summary>
    /// ProcessBar.xaml 的交互逻辑
    /// </summary>
    public partial class MProcessBar : UserControl
    {
        Color c1 = Color.FromArgb(0, 0, 0, 0);
        Color c2 = Color.FromArgb(0, 0, 0, 0);
        Thread t = null;
        bool isStop = true;
        

        public MProcessBar()
        {
            InitializeComponent();
            c1 = color1.Color;
            c2 = color2.Color;
        }

        public static DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MProcessBar), new PropertyMetadata(new CornerRadius(0)));
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        double processNum = 100;//0~100
        public double getProcess()
        {
            return processNum;
        }
        //进度设置
        public void updateProcess(double processNum)
        {
            if (processNum < 0) processNum = 0;
            if (processNum > 100) processNum = 100;
            this.processNum = processNum;
            if (Visibility != Visibility.Visible)
                return;
            int p = (int)processNum;
            double w = (border_bk.ActualWidth * processNum / 100.0);
            if (w > whiteBorder0.ActualWidth)
                w = whiteBorder0.ActualWidth + 0.2;
            whiteBorder.Width = w;
        }

        //滚动条渐变动画
        double offset = 0;
        public void updateOffset()
        {
            color2.Offset = offset;
            offset += 0.03;
            if (offset > 1)
            {
                offset = 0;
                color1.Color = c2;
                color2.Color = c1;
                color3.Color = c2;
                c1 = color1.Color;
                c2 = color2.Color;
                color2.Offset = offset;
            }
        }


        public void Start()
        {
            if(t != null && t.ThreadState == ThreadState.Running)
            {
                return;
            }
            t = new Thread(new ThreadStart(ThreadHandle));
            t.Start();
            isStop = false;
        }
        public void Stop()
        {
            isStop = true;
        }

        public void ThreadHandle()
        {
            while(isStop == false)
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,new Action(updateOffset));
             
                Thread.Sleep(100);
            }
        }
    }
}
