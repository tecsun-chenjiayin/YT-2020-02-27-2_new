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
    /// MyCheckBox.xaml 的交互逻辑
    /// </summary>
    public partial class MyCheckBox : UserControl
    {
        public MyCheckBox()
        {
            InitializeComponent();
        }

        public bool click()
        {
            if (CheckedMark.Visibility != Visibility.Visible)
                CheckedMark.Visibility = Visibility.Visible;
            else
                CheckedMark.Visibility = Visibility.Collapsed;
            return CheckedMark.Visibility == Visibility.Visible;
        }

        public bool isSelect()
        {
            return CheckedMark.Visibility == Visibility.Visible;
        }
    }
}
