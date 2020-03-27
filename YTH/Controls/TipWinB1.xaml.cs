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

namespace YTH.Controls
{
    /// <summary>
    /// Bussiness1下使用的提示
    /// </summary>
    public partial class TipWinB1 : Window
    {
        static TipWinB1 obj = null;
        Action nextStep = null;
        ThreadProperty tp = null;
        ThreadProperty uiTp = null;
        TextBlock tb = null;
        public TipWinB1()
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
            if (obj == null)
                obj = new TipWinB1();
        }

        static string tip_ = "";
        public static void showTip(string tip, ulong keepTime, Action nextStep)
        {
            if (obj == null)
                return;
            tip_ = tip;
            obj.nextStep = nextStep;
            obj.tp.resetTime(keepTime);
            obj.uiTp.start();
        }

        private static void show()
        {
            obj.border.Child = obj.tb;
            obj.tb.Text = tip_;
            obj.Show();
            obj.tp.start();
        }

        private static void hidenTip()
        {
            if (obj == null) return;
            obj.border.Child = null;
            obj.tipValue.Text = "";
            obj.UpdateLayout();
            obj.Hide();
            if (obj.nextStep != null)
                obj.nextStep();
        }

        public static void close()
        {
            if (obj != null)
                obj.Close();
        }

    }
}
