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
using YTH.ZhanJiang.BuKa;

namespace YTH.ZhanJiang
{
    /// <summary>
    /// MakeCardProcess2.xaml 的交互逻辑
    /// </summary>
    public partial class MakeCardProcess2 : UserControl
    {
        public MakeCardProcess2()
        {
            InitializeComponent();
        }

        static MakeCardProcess2 self = null;
        public static MakeCardProcess2 GetObject()
        {
            if (self == null)
                self = new MakeCardProcess2();
            return self;
        }

        private void TXButton_Click(object sender, RoutedEventArgs e)
        {
            newCard.GetObject().Goin();
        }
    }
}
