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

namespace WpfControlLibrary
{
    /// <summary>
    /// InputPhoneNum.xaml 的交互逻辑
    /// </summary>
    public partial class InputPhoneNum : UserControl
    {
        public delegate void OK_Click(string psw);
        Action cancel = null;
        OK_Click ok = null;
        public InputPhoneNum()
        {
            InitializeComponent();
        }
        public void init(Action cancel, OK_Click ok)
        {
            this.ok = ok;
            this.cancel = cancel;
        }

        private void WrapPanel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TXButton btn = (TXButton)e.Source;
                string tag = btn.Tag.ToString();
                if (tag == "Num")
                {
                    if (phone.Text.Length < phone.MaxLength)
                    {
                        phone.Text += btn.Content.ToString();
                    }
                    tip.Text = "";
                }
                else if (tag == "ok")
                {
                    if (phone.Text.Length == phone.MaxLength)
                    {
                        if (ok != null)
                            ok(phone.Text);
                    }
                    else
                        tip.Text = "提示：电话号码长度不足！";
                }
                else if (tag == "close")
                {
                    if (cancel != null)
                        cancel();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        public void clear()
        {
            phone.Text = "";
            tip.Text = "";
        }
    }
}
