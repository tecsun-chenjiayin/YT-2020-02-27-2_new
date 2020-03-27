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
    /// Loading2.xaml 的交互逻辑
    /// </summary>
    public partial class Loading2 : UserControl
    {
        public Loading2()
        {
            InitializeComponent();
        }

        public static DependencyProperty ScaleYProperty = DependencyProperty.Register("ScaleY", typeof(double), typeof(Loading2), new PropertyMetadata(0.5));
        public double ScaleY
        {
            get { return (double)GetValue(ScaleYProperty); }
            set
            {
                SetValue(ScaleYProperty, value);
            }
        }

        public static DependencyProperty ScaleXProperty = DependencyProperty.Register("ScaleX", typeof(double), typeof(Loading2), new PropertyMetadata(0.5));
        public double ScaleX
        {
            get { return (double)GetValue(ScaleXProperty); }
            set
            {
                SetValue(ScaleXProperty, value);
            }
        }
    }
}
