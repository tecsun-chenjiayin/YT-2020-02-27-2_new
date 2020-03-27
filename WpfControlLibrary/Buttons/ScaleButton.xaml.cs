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
    /// ScaleButton.xaml 的交互逻辑
    /// </summary>
    public partial class ScaleButton : Button
    {
        public ScaleButton()
        {
           
            InitializeComponent();
        }

        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ScaleButton), new PropertyMetadata(""));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static DependencyProperty TextMarginProperty = DependencyProperty.Register("TextMargin", typeof(string), typeof(ScaleButton), new PropertyMetadata("0"));
        public string TextMargin
        {
            get { return (string)GetValue(TextMarginProperty); }
            set { SetValue(TextMarginProperty, value); }
        }

        public static DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(ImageSource), typeof(ScaleButton), new PropertyMetadata(null));
        public ImageSource Path
        {
            set { SetValue(PathProperty, value); }
            get { return (ImageSource)GetValue(PathProperty); }
        }
    }
}
