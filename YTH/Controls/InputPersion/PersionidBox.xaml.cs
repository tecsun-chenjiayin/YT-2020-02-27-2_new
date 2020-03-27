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

namespace YTH.Controls.InputPersion
{
    /// <summary>
    /// PersionidTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class PersionidTextBox : UserControl
    {
        static InputPersionidWin inputWin = null;

        public PersionidTextBox()
        {
            InitializeComponent();
        }
        public void reset()
        {
            persionID.Foreground = Brushes.Gray;
            persionID.Text = "请输入";
        }
        public string getPerionid()
        {
            if (persionID.Text == "请输入")
                return "";
            else
                return persionID.Text;
        }

        private void UserControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (inputWin == null)
                inputWin = new InputPersionidWin();
            inputWin.show(ok);
        }

        private void ok(string persionid)
        {
            persionID.Foreground = Brushes.Black;
            persionID.Text = persionid;
        }
    }
}
