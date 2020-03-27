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
    /// CharacterButton.xaml 的交互逻辑
    /// </summary>
    public partial class CharacterButton : UserControl
    {
        public CharacterButton()
        {
            InitializeComponent();
        }

        static DependencyProperty Text1Property = DependencyProperty.Register("Text1", typeof(string), typeof(CharacterButton), new PropertyMetadata("A"));
        public string Text1 { get { return (string)GetValue(Text1Property); } set { SetValue(Text1Property, value); } }

        static DependencyProperty Text2Property = DependencyProperty.Register("Text2", typeof(string), typeof(CharacterButton), new PropertyMetadata("A"));
        public string Text2 { get { return (string)GetValue(Text2Property); } set { SetValue(Text2Property, value); } }

        static DependencyProperty IsNumProperty = DependencyProperty.Register("IsNum", typeof(bool), typeof(CharacterButton), new PropertyMetadata(true));
        public bool IsNum { get { return (bool)GetValue(IsNumProperty); }
            set
            {
                if(value != IsNum)
                {
                    Brush b1 = num.Foreground;
                    Brush b2 = character.Foreground;
                    character.Foreground = b1;
                    num.Foreground = b2;
                    SetValue(IsNumProperty, value);
                }
            }
        }


        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
