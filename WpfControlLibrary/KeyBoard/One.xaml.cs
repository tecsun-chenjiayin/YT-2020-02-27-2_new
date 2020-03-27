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

namespace WpfControlLibrary.KeyBoard
{
    /// <summary>
    /// One.xaml 的交互逻辑
    /// </summary>
    public partial class One : UserControl
    {
        public One()
        {
            InitializeComponent();
            
        }

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        public static DependencyProperty zProperty = DependencyProperty.Register("z", typeof(string), typeof(One), new PropertyMetadata("啊"));
        public string z
        {
            get { return (string)GetValue(zProperty); }
            set { SetValue(zProperty, value); }
        }

        public static DependencyProperty numProperty = DependencyProperty.Register("num", typeof(string), typeof(One), new PropertyMetadata("8"));
        public string num
        {
            get { return (string)GetValue(numProperty); }
            set { SetValue(numProperty, value); }
        }
    }
}
