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

namespace YTH
{
    /// <summary>
    /// TXButton2.xaml 的交互逻辑
    /// </summary>
    public partial class TXButton2 : UserControl
    {
        public TXButton2()
        {
            InitializeComponent();
        }
        static TXButton2()
        {
        }
        public static DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",   //属性名称
                typeof(string),      //属性类型
                typeof(TXButton2),   //属性所有者
                new PropertyMetadata(""),//属性默认值
                new ValidateValueCallback(callBackFun));//设置属性时候的属性值校验函数（可无）

        static bool callBackFun(object val)
        {
            return true;
        }
        public Brush MouseOverBKGround
        {
            get { return (Brush)GetValue(TextProperty); }
            set { SetValue(TextProperty, value);
                tx.Content = value;
            }
        }
    }
}
