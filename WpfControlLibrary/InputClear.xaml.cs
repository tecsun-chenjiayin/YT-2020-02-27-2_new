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
    /// InputClear.xaml 的交互逻辑
    /// </summary>
    public partial class InputClear : UserControl
    {
        public InputClear()
        {
            InitializeComponent();
        }

        public static DependencyProperty TipFontSizeProperty =
            DependencyProperty.Register("TipFontSize", typeof(double), typeof(InputClear), new PropertyMetadata(20.0));
        public double TipFontSize
        {
            set { SetValue(TipFontSizeProperty, value); }
            get { return (double)GetValue(TipFontSizeProperty); }
        }

        public static DependencyProperty TipTextProperty =
    DependencyProperty.Register("TipText", typeof(string), typeof(InputClear), new PropertyMetadata("默认提示内容"));
        public string TipText
        {
            set { SetValue(TipTextProperty, value); }
            get { return (string)GetValue(TipTextProperty); }
        }

        public static DependencyProperty TextProperty =
DependencyProperty.Register("Text", typeof(string), typeof(InputClear), new PropertyMetadata(""));
        public string Text
        {
            set { SetValue(TextProperty, value); inputValue.Text = value; }
            get { return inputValue.Text; }
        }

        public static DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(int), typeof(InputClear), new PropertyMetadata(100));
        public int MaxLength
        {
            set { SetValue(MaxLengthProperty, value); }
            get { return (int)GetValue(MaxLengthProperty); }
        }

        private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            inputValue.Clear();
        }

        public void Clear()
        {
            inputValue.Clear();
        }
    }
}
