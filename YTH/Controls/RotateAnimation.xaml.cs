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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace YTH.Controls
{
    /// <summary>
    /// 旋转动画，无限循环
    /// </summary>
    public partial class RotateAnimation : UserControl
    {
        DoubleAnimation dd = null;
        TransformGroup group = new TransformGroup();
        RotateTransform angle = new RotateTransform();
        public RotateAnimation()
        {
            InitializeComponent();
        }

        //一个循环持续多少秒
        public void init(int duration, string imagePath)
        {
            image.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));// 就绝对路径 
            dd = new DoubleAnimation(0, 360, new Duration(TimeSpan.FromSeconds(duration)), FillBehavior.Stop);
            dd.RepeatBehavior = RepeatBehavior.Forever;
            group.Children.Add(angle);
            //image.RenderTransform = group;
            image.RenderTransformOrigin = new Point(0.5, 0.5);
            angle.BeginAnimation(RotateTransform.AngleProperty, dd);
        }
        public void start()
        {
            Visibility = Visibility.Visible;
            image.RenderTransform = group;
        }
        public void stop()
        {
            image.RenderTransform = null;//恢复初始状态，并非停留在旋转后的某个角度
            Visibility = Visibility.Hidden;
        }

    }
}
