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

namespace YTH.ZhanJiang
{
    /// <summary>
    /// SelectCardStyle.xaml 的交互逻辑
    /// </summary>
    public partial class SelectCardStyle : UserControl
    {
        public delegate void NEXT(string style);
        NEXT nextStep;
        public SelectCardStyle()
        {
            InitializeComponent();
        }

        static SelectCardStyle self = null;
        public static SelectCardStyle GetObject()
        {
            if (self == null)
                self = new SelectCardStyle();
            return self;
        }

        public void Goin(NEXT nextStep)
        {
            this.nextStep = nextStep;
            Functions.CD.business1.setBusinessValue(this);
        }

        private void styl2_Click(object sender, RoutedEventArgs e)
        {
            nextStep("2");
        }

        private void styl3_Click(object sender, RoutedEventArgs e)
        {
            nextStep("3");
        }

        private void styl1_Click(object sender, RoutedEventArgs e)
        {
            nextStep("1");
        }
    }
}
