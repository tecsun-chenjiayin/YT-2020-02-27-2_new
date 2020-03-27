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
    /// LRButton.xaml 的交互逻辑
    /// </summary>
    public partial class LRButton : Button
    {
        public LRButton()
        {
            InitializeComponent();
        }

        public static DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(ImageSource), typeof(LRButton), new PropertyMetadata(null));
        public ImageSource Path
        {
            set { SetValue(PathProperty, value); }
            get { return (ImageSource)GetValue(PathProperty); }
        }
    }
}
