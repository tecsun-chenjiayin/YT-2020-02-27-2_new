using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// TXButton.xaml 的交互逻辑
    /// </summary>
    public partial class TXButton : Button
    {
        public TXButton()
        {
            InitializeComponent();
         
        }
        static TXButton()
        {
        }

        static Brush normalBrush = Brushes.Black;
        public static DependencyProperty MouseOverBKGroundProperty = 
            DependencyProperty.Register(
                "MouseOverBKGround",   //属性名称
                typeof(Brush),      //属性类型
                typeof(TXButton),   //属性所有者
                new PropertyMetadata(normalBrush),//属性默认值
                new ValidateValueCallback(callBackFun));//设置属性时候的属性值校验函数（可无）

        static bool callBackFun(object val)
        {
            return true;
        }
        public Brush MouseOverBKGround
        {
            get { return (Brush)GetValue(MouseOverBKGroundProperty); }
            set { SetValue(MouseOverBKGroundProperty, value); }
        }

        public static DependencyProperty MousePressBKGroundProperty =
            DependencyProperty.Register(
                "MousePressBKGround",
                typeof(Brush),
                typeof(TXButton),
                new PropertyMetadata(normalBrush),
                new ValidateValueCallback(MouseOverBKGroundCallBackFun));
        public Brush MousePressBKGround
        {
            get { return (Brush)GetValue(MousePressBKGroundProperty); }
            set { SetValue(MousePressBKGroundProperty, value); }
        }

        static bool MouseOverBKGroundCallBackFun(object val)
        {
            return true;
        }

        public static DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof(CornerRadius), typeof(TXButton), new PropertyMetadata(new CornerRadius(0)), new ValidateValueCallback(CornerRadiusCallBackFun));
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value);}
        }
        static bool CornerRadiusCallBackFun(object val)
        {
            return true;
        }

        public static readonly DependencyProperty PressedForegroundProperty =
            DependencyProperty.Register("PressedForeground", typeof(Brush), typeof(FButton), new PropertyMetadata(Brushes.White));
        public Brush PressedForeground
        {
            get { return (Brush)GetValue(PressedForegroundProperty); }
            set { SetValue(PressedForegroundProperty, value); }
        }

        //public static readonly DependencyProperty MouseOverForegroundProperty =
        //    DependencyProperty.Register("MouseOverForeground", typeof(Brush), typeof(FButton), new PropertyMetadata(Brushes.White));
        //public Brush MouseOverForeground
        //{
        //    get { return (Brush)GetValue(MouseOverForegroundProperty); }
        //    set { SetValue(MouseOverForegroundProperty, value); }
        //}
    }
}
