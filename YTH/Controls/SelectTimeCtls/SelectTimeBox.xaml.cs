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

namespace YTH.Controls.SelectTimeCtls
{
    /// <summary>
    /// SelectTimeBox.xaml 的交互逻辑
    /// </summary>
    public partial class SelectTimeBox : UserControl
    {
        public SelectTimeBox()
        {
            InitializeComponent();
        }

        public void reset()
        {
            tb.Text = "请选择";
            tb.Foreground = Brushes.Gray;
        }

        public void setDateToNow()
        {
            tb.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            tb.Foreground = Brushes.Black;
        }

        public string getTime()
        {
            if (tb.Text == "请选择")
                return "";
            else
                return tb.Text;
        }

        static SelectTimeWin stw = null;

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (stw == null)
                stw = new SelectTimeWin();
            stw.show(tb);
        }


    }
}
