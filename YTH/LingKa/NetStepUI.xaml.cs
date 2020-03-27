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

namespace YTH.LingKa
{
    /// <summary>
    /// NetStepUI.xaml 的交互逻辑
    /// </summary>
    public partial class NetStepUI : UserControl
    {
        public NetStepUI()
        {
            InitializeComponent();
        }

        static NetStepUI nsui = null;

        Action nextStep = null;
        public static void show(Action nextStep, string tip, bool isEnd)
        {
            TH.addOnceUI(new Action(() => {
                if (nsui == null)
                    nsui = new NetStepUI();
                nsui.t.Text = tip;
                nsui.nextStep = nextStep;
                if (isEnd)
                    nsui.ok.Content = "返回首页";
                else
                    nsui.ok.Content = "领取下一张";
                CD.business1.setBusinessValue(nsui);
            }));
        }

        private void ok_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (nextStep != null)
                nextStep();
        }

        private void TXButton_Click(object sender, RoutedEventArgs e)
        {
            if (nextStep != null)
                nextStep();
        }
    }
}
