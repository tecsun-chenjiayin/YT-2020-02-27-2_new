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
    /// InputPhoneNum.xaml 的交互逻辑
    /// </summary>
    public partial class InputPhoneNum : UserControl
    {
        public static string phone = "";
        public InputPhoneNum()
        {
            InitializeComponent();
        }

        static InputPhoneNum self = null;
        public static InputPhoneNum GetObject()
        {
            if (self == null)
                self = new InputPhoneNum();
            return self;
        }

        Action nextStep = null;
        //入口
        public void Goin(Action nextStep_)
        {
            nextStep = nextStep_;
            tb.Text = "";
            KeyPad.startInput(input, Delete, clear, OK, Back_, this);
            CD.business1.setBusinessValue(this);
        }

        //输入
        private void StackPanel_Click(object sender, RoutedEventArgs e)
        {
            FButton fb = (FButton)e.Source;
            if (fb.Content == null) return;
            string val = fb.Content.ToString();
            if (val.Length != 1) return;
            input(val);
        }
        private void input(string val)
        {
            if (Visibility != Visibility.Visible) return;
            if (tb.Text.Length < tb.MaxLength && val.Length == 1 && val[0] >= '0' && val[0] <= '9')
                tb.Text += val;
            if (tb.Text.Length == tb.MaxLength)
                border.BorderBrush = Brushes.Black;
            else
                border.BorderBrush = Brushes.Red;
        }
        //删除
        private void Delete()
        {
            if (Visibility != Visibility.Visible) return;
            delete_Click(null, null);
        }
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if(tb.Text.Length > 0)
            {
                tb.Text = tb.Text.Substring(0, tb.Text.Length - 1);
            }
        }
        //确认
        private void OK()
        {
            if (Visibility != Visibility.Visible || Parent == null) return;
            ok_Click(null, null);
        }
        private void ok_Click(object sender, RoutedEventArgs e)
        {
            phone = tb.Text;
            if (tb.Text.Length == tb.MaxLength)
                nextStep();
            else
                ShowTip.show(false, null, "请输入11位手机号");

        }
        //清除
        private void clear()
        {
            if (Visibility != Visibility.Visible) return;
            tb.Text = "";
        }
        //返回
        private void Back_()
        {
            return;
            //if (Visibility != Visibility.Visible) return;
            //BackExit.Back();
        }
    }
}
