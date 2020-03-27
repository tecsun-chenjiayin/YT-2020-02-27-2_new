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
    /// SelectTimeBox.xaml 的交互逻辑
    /// </summary>
    public partial class SelectTimeBox : UserControl
    {
        int style = 3;//3年月日  2年月 1年
        public SelectTimeBox()
        {
            InitializeComponent();
        }

        public void reset(bool selectMonth, bool selectDay)
        {
            tb.Text = DateTime.Now.ToString("yyyy-MM");
            tb.Foreground = Brushes.Gray;
            if (selectMonth)
            {
                style = 2;
                if (selectDay)
                    style = 3;
            }
            else
                style = 1;
        }

        public void setDateToNow()
        {
            if(style == 1)
                tb.Text = System.DateTime.Now.ToString("yyyy");
            if (style == 2)
                tb.Text = System.DateTime.Now.ToString("yyyy-MM");
            if (style == 3)
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
            stw.show(tb, style);
        }


    }
}
