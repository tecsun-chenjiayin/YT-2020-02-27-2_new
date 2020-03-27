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

namespace YTH.Controls
{
    /// <summary>
    /// ShowError1.xaml 的交互逻辑
    /// </summary>
    public partial class ShowError : UserControl
    {
        static ShowError se = null;
        static string em = null;
        public ShowError()
        {
            InitializeComponent();
        }

        private void back_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Functions.BackExit.Exit();
        }

        public static void show1(string errorMsg)
        {
            em = errorMsg;
            Functions.TH.addOnceUI(show_);
        }

        private static void show_()
        {
            if (se == null)
                se = new ShowError();
            se.error.Text = em;
            Functions.CD.business1.setBusinessValue(se);
        }

        static string error2 = "";
        public static void show2(string errorMsg)
        {
            error2 = errorMsg;
            Functions.TH.addOnceUI(show_2);

        }

        public static void show_2()
        {
            if (se == null)
                se = new ShowError();
            se.error.Text = error2;
            Functions.CD.business2.setBusinessValue(se);
        }
    }
}
