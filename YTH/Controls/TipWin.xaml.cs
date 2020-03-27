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
using System.Windows.Shapes;
using YTH.Functions;
using YTH.GetCar.Self;

namespace YTH
{
    /// <summary>
    /// TipWin.xaml 的交互逻辑
    /// </summary>
    public partial class TipWin : Window
    {
        private static TipWin tw = null;

        ThreadProperty uiTp = null;
        ThreadProperty tp = null;
        Action nextStep = null;
        TextBlock tb = null;
        private TipWin()
        {
            InitializeComponent();
            // 设置全屏
            this.WindowState = System.Windows.WindowState.Normal;
            this.WindowStyle = System.Windows.WindowStyle.None;
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
            //this.Topmost = true;
            this.Left = 0.0;
            this.Top = 0.0;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;

            ShowInTaskbar = false;
            tp = new ThreadProperty(100, true, false, hidenTip, this);
            uiTp = new ThreadProperty(50, true, false, show, this);
            tb = tipValue;
        }
        public static void init()
        {
            tw = new TipWin();
        }
        //显示提示信息=====================================
        static string tip_ = "";
        public static void showTip(string tip, ulong keepTime, Action nextStep)
        {
            if (tw == null)
                return;
            tip_ = tip;
            tw.tp.resetTime(keepTime);
            tw.nextStep = nextStep;
            tw.uiTp.start();
        }
        private static void show()
        {
            tw.image.stop();
            tw.tipBorder.Visibility = Visibility.Visible;
            tw.tipValue.Text = tip_;
            tw.Show();
            tw.tp.start();
        }

        //显示加载动画=====================================
        static ThreadProperty showTP = null;
        bool hasInit = false;
        public static void showLoading()
        {
            if (tw == null) return;
            if (showTP == null)
                showTP = new ThreadProperty(50, true, false, showLoading_, tw);
            showTP.start();
        }
        private static void showLoading_()
        {
            if (!tw.hasInit)
            {
                tw.image.init(3, ImagePaths.getPathByName("加载"));
                tw.hasInit = true;
            }
            tw.tipBorder.Visibility = Visibility.Hidden;
            tw.Show();
            tw.image.start();
        }

        //停止并隐藏加载动画=====================================
        static ThreadProperty stopTP = null;
        public static void stopLoading()
        {
            if (tw == null) return;
            if (stopTP == null)
                stopTP = new ThreadProperty(50, true, false, hidenTip, tw);
            stopTP.start();
        }
        private static void hidenTip()
        {
            if (tw == null) return;
            tw.tipValue.Text = "";
            tw.UpdateLayout();
            tw.Hide();
            if (tw.nextStep != null)
                tw.nextStep();
        }

        //关闭并释放窗口内存
        public static void close()
        {
            if (tw != null)
                tw.Close();
        }
    }
}
