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
    /// MTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class MTextBox : UserControl
    {
        public MTextBox()
        {
            InitializeComponent();
        }

        


        private void p1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (p1.Text.Length == 0)
                bk1.Text = "请输入用户名";
            else
                bk1.Text = "";
        }
    }
}
