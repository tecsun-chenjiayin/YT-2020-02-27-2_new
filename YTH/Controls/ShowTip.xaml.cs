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
using System.Windows.Shapes;
using YTH.Functions;

namespace YTH
{
    /// <summary>
    /// ShowTip.xaml 的交互逻辑
    /// </summary>
    public partial class ShowTip : Window
    {
        static Action nextStep = null;
        static BitmapImage success = null;
        static BitmapImage failed = null;
        static ShowTip st = null;
        public ShowTip()
        {
            InitializeComponent();
            Topmost = true;
        }

        //private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (nextStep != null)
        //        nextStep();
        //    Hide();
        //}

        public static void show(bool isSuccess, Action nextStep_, string txt)
        {
            nextStep = nextStep_;
            TH.addOnceUI(new Action(() => {
                if(success == null)
                {
                    success = new BitmapImage(new Uri(@"../Soruce/Inages_ZQ/成功.png", UriKind.Relative));
                    failed = new BitmapImage(new Uri(@"../Soruce/Images/失败.png", UriKind.Relative));
                }
                if (st == null)
                    st = new ShowTip();
                if (isSuccess)
                    st.ico.Source = success;
                else
                    st.ico.Source = failed;
                st.tip.Text = txt;
                st.Show();
            }));

        }

        private void TXButton_Click(object sender, RoutedEventArgs e)
        {
            if (nextStep != null)
                nextStep();
            Hide();
        }
    }
}
