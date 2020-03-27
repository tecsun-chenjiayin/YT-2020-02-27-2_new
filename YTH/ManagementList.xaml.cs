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

namespace YTH
{
    /// <summary>
    /// ManagementList.xaml 的交互逻辑
    /// </summary>
    public partial class ManagementList : UserControl
    {
        public ManagementList()
        {
            InitializeComponent();
        }

        public void Goin()
        {
            BackExit.setBack(Goin);
            CD.setMainUI(this);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            BackExit.Exit();
        }

        AddCard add = null;
        private void AddCar_Click(object sender, RoutedEventArgs e)
        {
            if (add == null)
                add = new AddCard();
            add.Goin();
        }
    }
}
