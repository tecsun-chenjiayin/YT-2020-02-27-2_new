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
    /// DateTime.xaml 的交互逻辑
    /// </summary>
    public partial class DateTime : UserControl
    {
        ThreadProperty updateTimeTP = null;
        public DateTime()
        {
            InitializeComponent();
            timeTB.Text = "今天是：" + TimeTag.GetTime2();
            updateTimeTP = new ThreadProperty(60000, false, false, update, this);
        }

        public void startUpdateThread()
        {
            updateTimeTP.start();
        }

        public void stopUpdateThread()
        {
            updateTimeTP.stop();
        }

        private void update()
        {
            timeTB.Text = "今天是：" + TimeTag.GetTime2();
        }
    }
}
