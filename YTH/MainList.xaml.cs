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
using YTH.ZhanJiang;
using YTH.ZhanJiang.BuKa;

namespace YTH
{
    /// <summary>
    /// MainList.xaml 的交互逻辑
    /// </summary>
    public partial class MainList : UserControl
    {
        public MainList()
        {
            InitializeComponent();
            
        }

        public void Goin()
        {
            time.start();
            BackExit.setBack(Goin);
            CD.setMainUI(this);
            list.ScrollIntoView(first);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            time.stop();
            BackExit.Back();
        }

        private void Xinbanka_Click(object sender, RoutedEventArgs e)
        {
            time.stop();
            newCard.GetObject().Goin();
        }
        private void Buka_Click(object sender, RoutedEventArgs e)
        {
            time.stop();
            YTH.ZhanJiang.buka.GetObject().Goin();
        }
        private void Huanka_Click(object sender, RoutedEventArgs e)
        {
            ShowTip.show(false, null, "功能尚未开放，敬请期待");
            //time.stop();
            //YTH.ZhanJiang.huanka.GetObject().Goin();
        }

        private void Lingka_Click(object sender, RoutedEventArgs e)
        {
            time.stop();
            ZhanJiang.LinagKa.GetObject().Goin();
        }

        private void Jindu_Click(object sender, RoutedEventArgs e)
        {
            time.stop();
            ZhanJiang.MakeCardProcess.GetObject().Goin();
        }

        private void Message_Click(object sender, RoutedEventArgs e)
        {
            time.stop();
            SearchData.GetObject().Goin();
        }

        private void YbBalance_Click(object sender, RoutedEventArgs e)
        {
            time.stop();
            SearchList.GetObject().Goin();
        }

        private void JrBalance_Click(object sender, RoutedEventArgs e)
        {
            //time.stop();
            ShowTip.show(false, null, "功能尚未开放，敬请期待");
        }

        private void ChangePsw_Click(object sender, RoutedEventArgs e)
        {
            time.stop();
            ChangePsw.GetObject().Goin();
        }

        private void ScrollViewer_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
    }
}
