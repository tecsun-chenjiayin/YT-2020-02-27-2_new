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
using YTH.Functions.ReadCarAndSQCode;

namespace YTH.BuKa
{
    /// <summary>
    /// InputPersionMsg.xaml 的交互逻辑
    /// </summary>
    public partial class InputPersionMsg : UserControl
    {
        public InputPersionMsg()
        {
            InitializeComponent();
        }
        Action nextStep = null;
        static InputPersionMsg self = null;
        public static void Goin(Action nextStep)
        {
            if (self == null)
                self = new InputPersionMsg();
            self.p1.Text = "";
            self.p2.Text = "";
            self.nextStep = nextStep;
            CD.setTopUI(self);
        }


        private void p1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (p1.Text.Length == 0)
                bk1.Text = "请输入用户名";
            else
                bk1.Text = "";
        }

        private void input_Click(object sender, RoutedEventArgs e)
        {

        }

        private void p2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (p2.Text.Length == 0)
                bk2.Text = "请输入用户名";
            else
                bk2.Text = "";
        }

        private void input2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            if(p1.Text.Length == p1.MaxLength && p2.Text != "")
            {
                ReadIDCar.persionid = p1.Text;
                ReadIDCar.name = p2.Text;
                nextStep();
                CD.setTopUI(null);
            }
        }

        private void read_Click(object sender, RoutedEventArgs e)
        {
            B_ReadIDCar.getObject().BeforeGoin(nextStep, true);
            B_ReadIDCar.getObject().Goin();
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            CD.setTopUI(null);
        }
    }
}
