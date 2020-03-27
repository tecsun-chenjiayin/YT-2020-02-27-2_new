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
using YTH.LingKa;

namespace YTH
{
    /// <summary>
    /// 业务区1
    /// </summary>
    public partial class Business : UserControl
    {
        public Business()
        {
            InitializeComponent();
        }

        public void setPointNames(string[] names)
        {
            nodeGraph.setPointNames(names);
            nodeGraph.setIndex(1);
        }

        private void returnToMain_Click(object sender, RoutedEventArgs e)
        {
            Status.isWorking = false;
            BackExit.Exit();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            Status.isWorking = false;
            BackExit.Back();
        }
        
        public void setIndex(int pointIndex)
        {
            nodeGraph.setIndex(pointIndex);
        }

        public void setTitle(string name)
        {
            title.Text = name;
        }

        public void setBusinessValue(object val)
        {
            UIElement ui = (UIElement)val;
            bussinessArea.Child = ui;
        }

        public void hidenBackAndExitBtn()
        {
            MBSP.Visibility = Visibility.Hidden;
        }

        public void showBackAndExitBtn()
        {
            MBSP.Visibility = Visibility.Visible;
        }

        public void start()
        {
            time.start();
        }

        public void stop()
        {
            time.stop();
        }

        public static void Init(Action back, string title, string[] selfNames)
        {
            CD.timeTag.updateTag();
            BackExit.setBack(back);
            if (CD.business1 == null) CD.business1 = new Business();
            CD.setMainUI(CD.business1);
            CD.business1.showBackAndExitBtn();
            CD.business1.setPointNames(selfNames);
            CD.business1.setTitle("办理领卡");
            CD.business1.start();
            CD.business1.setBoxStatus(false);
        }

        public static Action ok_write = null;
        public static Action re_write = null;

        private void writeOk_Click(object sender, RoutedEventArgs e)
        {
            ok_write();
        }

        private void rewrite_Click(object sender, RoutedEventArgs e)
        {
            re_write();
        }

        public void setBoxStatus(bool isWrite)
        {
            if(isWrite)
            {
                bussinessArea.Width = 1200;
                about_write.Visibility = Visibility.Visible;
            }
            else
            {
                bussinessArea.Width = 1536;
                about_write.Visibility = Visibility.Hidden;
            }
        }
    }
}
